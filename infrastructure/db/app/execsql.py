import psycopg2
import sys


# Try to connect
if not len(sys.argv) == 3:
    print("Usage: python execsql.py postgresql-hostname sqlscript.sql")
    exit

host=sys.argv[1]
script=sys.argv[2]
connection_string="host='{host}' port='5432' dbname='{dbname}' user='{user}' password='{password}'".format(
        host=host,
        dbname='issuing',
        user='user',
        password='user'
    )
print("Using connection string: {connection_string}".format(connection_string=connection_string))
connection = psycopg2.connect(connection_string)

script_content = open(script, "r").read()
print(script_content)
with connection.cursor() as cursor:
    cursor.execute(script_content)
    cursor.close()
    
connection.commit()
connection.close()
