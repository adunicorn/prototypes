echo "Creating schema..."
python /app/execsql.py $POSTGRESQL_SERVICE_HOST /app/sql/schema.sql
echo "Schema created!"

echo "Filling table..."
python /app/execsql.py $POSTGRESQL_SERVICE_HOST /app/sql/values.sql
echo "Table filled!"

echo "Filling Redis..."
python /app/load-into-redis.py
echo "Redis filled!"

tail -f /dev/null
