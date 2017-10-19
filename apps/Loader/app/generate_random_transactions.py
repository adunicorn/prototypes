currencies=['CHF', 'EUR', 'USD', 'GBP']
descriptions=[
    ("Cafe", 2.0),
    ("Pizza Margherita", 15.0),
    ("Pizza Napoli", 17.0),
    ("Hamburger", 10.0),

    ("Flight to Toronto", 1500.0),
    ("Flight to New York", 1300.0),
    ("Flight to Pistoia", 500.0),
    ("Flight to Lugano", 400.0),
    ("Flight to Tokyo", 2000.0),
    ("Flight to Osaka", 2500.0),
    ("Flight to Singapore", 2000.0),

    ("iPhone 6", 700.0),
    ("iPhone 7", 800.0),
    ("iPhone 8", 900.0),

    ("Nokia phone 71130", 200.0),
    ("Book", 12.0),
    ("Ikea table", 200.0),
    ("Ikea chair", 70.0),

    ("Renault Scenic", 25000.0),
    ("Ferrari Testarossa", 300000.0),
    ("Suzuki Swift", 29000.0),
    ("Swatch watch model B80", 200.0),

    ("ErgoDox Keyboard", 200.0),
    ("T-Shirt", 29.0),
    ("Dinner chez Nunzio", 25.0),
    ("Flowers", 30.0),
    ("Casino", 60.0),
    ("Taxi", 30.0),
    ("Grocery Shopping", 10.0),
    ("Purchase at Aldi Supermarket", 40.0),
    ("Purchase at Migros", 50.0),
    ("Ray Ban Glasses", 100.0),
    ("ASAP Studio licenses", 2000.0),
    ("Michele Cafaggi Show", 30.0),
    ("FoxTrail game", 30.0),
    ("Samsung TV", 2000.0),
    ("Mac Book Pro", 2600.0),
    ("Train ticket", 20.0),
    ("Swisscom Phone bill", 150.0),
    ("Parfume", 90.0),

    ("Thinkpad Laptop", 1290.0),
    ("Swimming pool entrance ticket", 10.0),
    ("Seeds for birds", 2.0),
    ("Cat food", 10.0),
    ("Dog food", 10.0),
    ("Vet bill", 90.0),
    ("Hospital bill", 90.0),
    ("Grand Hotel Firmani''s", 290.0),
    ("Caferra Hotel", 190.0),
    ("Holiday Inn", 160.0),

    ("Lollypop", 2.0),
    ("Purchase on Google Play", 10.0),
    ("Purchase on iTunes", 20.0),
]

import uuid
import random
from decimal import *

def get_one():
    item = descriptions[random.randrange(0, len(descriptions)-1)]
    currency = currencies[random.randrange(0, len(currencies)-1)]
    description = item[0]
    price = item[1]
    osc = 0.3
    variation = random.uniform(0, osc)
    final_price = price * (1 + variation)
    formatted_price="{0:.2f}".format(final_price)
    id = uuid.uuid4()

    return id, description, formatted_price, currency

