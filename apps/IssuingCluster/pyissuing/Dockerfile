FROM python:2-alpine
MAINTAINER Arialdo Martini arialdomartini@gmail.com

RUN pip install gunicorn==19.6.0


COPY ./app /app
WORKDIR /app


RUN pip install -r requirements.txt

EXPOSE 5000
USER 1001

#ENTRYPOINT ["/usr/local/bin/gunicorn"]
#CMD ["-w", "1", "-b", "0.0.0.0:5000", "--threads", "1", "app:app", "--access-logfile", "/dev/stdout", "--error-logfile", "/dev/stdout"]
CMD ["python", "app.py"]
