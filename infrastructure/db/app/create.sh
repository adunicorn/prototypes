echo "Creating schema..."
python /app/execsql.py $POSTGRESQL_SERVICE_HOST /app/sql/schema.sql

echo "Filling table..."
python /app/execsql.py $POSTGRESQL_SERVICE_HOST /app/sql/values.sql
tail -f /dev/null
