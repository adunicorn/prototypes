from flask import Flask
from flask import jsonify
from flask import make_response
import mock

app = Flask(__name__)


@app.route("/")
def info():
    return "PyIssuing v2"

@app.route("/api/transaction/<int:id>")
def get_transaction(id):
    resp = make_response(jsonify(get_transaction(id)))
    resp.headers["version"] = "2 new"
    return resp

def get_transaction(id):
    return {"id": 1, "amount": 100, "description": "some dummy value"}
    

if __name__ == '__main__':
    with mock.patch('pwd.getpwuid') as getpw:
        getpw.return_value = "foobar"
        app.run(host="0.0.0.0")
