FROM svlentink/psycopg2
RUN pip install requests redis
COPY ./app /app
USER 1001
ENTRYPOINT ["sh", "/app/create.sh"]