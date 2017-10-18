FROM mono
COPY src/RabbitConsumer/bin/Debug/ /program
WORKDIR /program
USER 1001
ENTRYPOINT ["mono", "RabbitConsumer.exe"]