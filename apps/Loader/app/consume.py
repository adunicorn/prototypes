import psycopg2
import sys
import os
import time

print("consume v6")

# Try to connect

host=os.getenv('POSTGRESQL_SERVICE_HOST')
connection_string="host='{host}' port='5432' dbname='{dbname}' user='{user}' password='{password}'".format(
        host=host,
        dbname='issuing',
        user='user',
        password='user'
    )

print("Using connection string: {connection_string}".format(connection_string=connection_string))
connection = psycopg2.connect(connection_string)

with connection.cursor() as cursor:
    cursor.execute("SELECT count(*) as count FROM transactions")
    record = cursor.fetchone()
    nRows = record[0]
    print("Total number of transactions: {nRows}".format(nRows=nRows))
    cursor.close()


with connection.cursor() as cursor:
    while True:
        cursor.execute("SELECT description, amount FROM transactions LIMIT 1 OFFSET floor( random() * %s )", [nRows])
        record = cursor.fetchone()
        print("Amount: {amount}\t\tDescription: {description}".format(description=record[0], amount=record[1]))
        time.sleep(.300)
        connection.commit()

connection.close()
print("Done")
