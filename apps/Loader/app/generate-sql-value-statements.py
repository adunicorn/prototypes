import sys
import os
import time
import generate_random_transactions

for i in range(1, 1001):
    id, description, price, currency = generate_random_transactions.get_one()
    insert = "insert into transactions(id, description, amount, currency) VALUES('{id}', '{description}', '{price}', {currency});".format(id = i, description = description, price = price, currency = currency)
    print(insert)
