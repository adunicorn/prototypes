from flask import Flask, abort
from flask import jsonify
from flask import make_response
import mock
import redis
import json
import os


host = os.getenv('REDIS_MASTER_SERVICE_HOST')
#host = 'localhost'
rs = redis.Redis(host=host, password='redis')

version="gunicorn v1"

print(version)

app = Flask(__name__)

shutting_down = False

@app.route("/")
def info():
    info = rs.info()
    return jsonify(info)
    return version


@app.route("/is_ready")
def is_ready():
    return get_transaction(3)


@app.route("/fail")
def this_fails():
    return "Fails", 503

@app.route("/api/transaction/<int:id>")
def get_transaction(id):
    resp = make_response(jsonify(read_transaction(id)))
    resp.headers["version"] = version
    return resp


def read_transaction(id):
    if id == 1:
        return {"id": 1, "amount": 100, "description": "some dummy value"}
    else:
        rs = redis.Redis(host=host, password='redis')
        record = rs.get("transaction_%s" % id)
        return json.loads(record)



if __name__ == '__main__':

    with mock.patch('pwd.getpwuid') as getpw:
        getpw.return_value = "foobar"
        app.run(host="0.0.0.0")
