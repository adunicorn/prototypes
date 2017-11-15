FROM ubuntu
MAINTAINER Arialdo Martini arialdomartini@gmail.com

RUN apt-get update -qqy
RUN apt-get upgrade -qqy
RUN apt-get install python python-dev build-essential python-pip -qqy
#RUN pip install virtualenv

RUN chmod g+w /etc/passwd

COPY adduser.sh /setup/adduser.sh
RUN useradd -m -p xxx -s /bin/bash 1001 -u 1001

COPY ./app /app
WORKDIR /app

RUN pip install -r requirements.txt
EXPOSE 5000
USER 1001
CMD ["python", "service.py"]
