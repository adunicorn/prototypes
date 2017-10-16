FROM mono:latest as monobuilder
COPY src/ /src/
RUN nuget restore /src/RabbitConsumer.sln
RUN msbuild /src/RabbitConsumer.sln


FROM mono
COPY --from=monobuilder src/RabbitConsumer/bin/Debug/ /program
WORKDIR /program
USER 1001
ENTRYPOINT ["mono", "RabbitConsumer.exe"]