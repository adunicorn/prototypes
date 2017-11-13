import psycopg2
import sys
import os
import time
import generate_random_transactions
import redis
import json

print("load-into-redis v1")
host = os.getenv('REDIS_MASTER_SERVICE_HOST')

def save_one_transaction(id):
    id_unused, description, price, currency = generate_random_transactions.get_one()
    redis_db = redis.StrictRedis(host=host, password='redis', port=6379, db=0)

    data = {"id" : str(id), "description": description, "amount": str(price), "currency": currency}
    data_json = json.dumps(data)
    print(data_json)
    redis_db.set("transaction_%s" % i, data_json)


# Try to connect
for i in range(1, 1000):
    save_one_transaction(i)


