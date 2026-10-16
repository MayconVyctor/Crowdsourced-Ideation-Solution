FROM liquibase/liquibase:4.29-alpine

USER root
RUN wget -q https://repo1.maven.org/maven2/com/mysql/mysql-connector-j/8.4.0/mysql-connector-j-8.4.0.jar -P /liquibase/lib/
USER liquibase

WORKDIR /liquibase/changelog

COPY . /liquibase/changelog/

ENTRYPOINT ["liquibase"]
CMD ["--changelog-file=db.changelog-master.xml", "update"]
