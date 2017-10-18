FROM mono
COPY /bin/Debug /program
WORKDIR /program
USER 1001
EXPOSE 5000
ENTRYPOINT ["mono", "OldIssuingService.exe"]
