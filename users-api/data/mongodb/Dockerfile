
FROM maven:3.9-eclipse-temurin-21 AS dependencies

WORKDIR /build

RUN printf '%s\n' \
    '<project xmlns="http://maven.apache.org/POM/4.0.0">' \
    '  <modelVersion>4.0.0</modelVersion>' \
    '  <groupId>jalau.usersapi</groupId>' \
    '  <artifactId>liquibase-mongodb-dependencies</artifactId>' \
    '  <version>1.0.0</version>' \
    '  <dependencies>' \
    '    <dependency>' \
    '      <groupId>org.liquibase.ext</groupId>' \
    '      <artifactId>liquibase-mongodb</artifactId>' \
    '      <version>5.0.4</version>' \
    '    </dependency>' \
    '  </dependencies>' \
    '</project>' > pom.xml

RUN mvn -B dependency:copy-dependencies \
    -DincludeScope=runtime \
    -DoutputDirectory=/dependencies \
    -DexcludeArtifactIds=liquibase-core,snakeyaml,commons-text,opencsv,commons-io

FROM liquibase/liquibase:5.0.4

COPY --from=dependencies /dependencies/ /liquibase/lib/
COPY changelog/ /liquibase/changelog/