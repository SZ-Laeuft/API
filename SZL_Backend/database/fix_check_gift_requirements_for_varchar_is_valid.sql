-- Fixes PostgreSQL error 42883:
-- "operator does not exist: character varying = boolean"
--
-- rounds.is_valid is intentionally varchar(255). The trigger function currently uses:
--   r.is_valid = true
-- PostgreSQL cannot compare varchar with boolean. This patch preserves the existing
-- function body and only replaces that comparison with a text comparison.

DO $$
DECLARE
    function_oid oid;
    function_sql text;
    patched_sql text;
BEGIN
    SELECT p.oid
    INTO function_oid
    FROM pg_proc p
    JOIN pg_namespace n ON n.oid = p.pronamespace
    WHERE p.proname = 'check_gift_requirements'
      AND pg_get_function_arguments(p.oid) = ''
    ORDER BY n.nspname = 'public' DESC
    LIMIT 1;

    IF function_oid IS NULL THEN
        RAISE EXCEPTION 'Function check_gift_requirements() was not found';
    END IF;

    function_sql := pg_get_functiondef(function_oid);

    patched_sql := replace(
        function_sql,
        'r.is_valid = true',
        'lower(trim(r.is_valid)) = ''true'''
    );

    IF patched_sql = function_sql THEN
        RAISE EXCEPTION 'Expected comparison "r.is_valid = true" was not found in check_gift_requirements()';
    END IF;

    EXECUTE patched_sql;
END;
$$;
