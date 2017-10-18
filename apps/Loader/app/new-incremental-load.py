import psycopg2
import sys
import os
import time
import generate_random_transactions
import requests
import json

print("new-incremental-load v4")

def send_one_request():
    id, description, price, currency = generate_random_transactions.get_one()

    host = "http://{host}:{port}".format(host=os.getenv('ISSUING_SERVICE_HOST'), port=5000)
    #host = "http://issuing.192.168.64.11.nip.io/"
    data = {"id" : str(id), "description": description, "amount": str(price), "currency": currency}
    data_json = json.dumps(data)
    url = host + "/api/transaction"
    headers={'Content-Type': 'application/json'}
    print("Posting {data_json} to {url}".format(data_json=data_json, url=url))
    response = requests.post(url, data=data_json, headers=headers)
    print(response.text)


# Try to connect
while True:
    send_one_request()


