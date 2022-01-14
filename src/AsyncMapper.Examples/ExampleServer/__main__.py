from faker import Faker
from flask import Flask, request, Response
import time
from datetime import datetime
from dataclasses import dataclass, field, is_dataclass, asdict
import json
import random

app = Flask(__name__)


fake = Faker()
fake.name()


@dataclass()
class Person:
    name: str = field(default_factory=fake.name)
    login: str = None
    phone: str = field(default_factory=fake.phone_number)
    address: str = field(default_factory=fake.address)
    job: str = field(default_factory=fake.job)
    employment_date: datetime = field(default_factory=fake.date_time_this_decade)

    def __post_init__(self):
        self.login = "".join(s[0] for s in self.name.split()).lower()


@dataclass()
class Task:
    id: int
    responsible_login: str
    description: str = field(default_factory=fake.text)
    finished: bool = field(default_factory=lambda: bool(random.getrandbits(1)))


people = list()
tasks = list()


def fill_database(n: int):
    for _ in range(n):
        people.append(Person())
    for i in range(n * 5):
        t = Task(i, random.choice(people).login)
        tasks.append(t)


def to_json(o):
    def json_default(o):
        if isinstance(o, datetime):
            return o.isoformat()
        elif isinstance(o, str):
            return o
        elif is_dataclass(o):
            return asdict(o)

    return json.dumps(o, default=json_default)


@app.before_request
def before():
    if "no-delay" not in request.headers:
        time.sleep(random.randint(2, 4))


def text_mimetipe(f):
    def decorated(f, *args, **kwargs):
        res = f(*args, **kwargs)
        return Response(res, content_type="text/json")


@app.route("/logins")
def logins():
    return to_json([p.login for p in people])


@app.route("/person")
def person():
    id_ = request.args.get("id")
    if id_ == "all":
        return to_json(people)
    elif id_:
        return to_json(people[int(id_)])
    login = request.args.get("login")
    prop = request.args.get("prop")
    if login:
        return str(next((getattr(p, prop, "null") for p in people if p.login == login), "null"))
    return "null"


@app.route("/task")
def task():
    id_ = request.args.get("id")
    if id_ == "all":
        return to_json(tasks)
    elif id_:
        return to_json(tasks[int(id_)])
    responsible_login = request.args.get("responsible_login")
    if responsible_login:
        return to_json(filter(lambda t: t.responsible_login == responsible_login, tasks))
    return "null"

@app.route("/test")
def test():
    return "daasd"

if __name__ == '__main__':
    fill_database(100)
    app.run()
