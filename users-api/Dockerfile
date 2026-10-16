FROM maven:3.9.9-eclipse-temurin-21 AS build
WORKDIR /app

COPY pom.xml .
COPY domain/pom.xml domain/
COPY application/pom.xml application/
COPY infrastructure/mysql/pom.xml infrastructure/mysql/
COPY presentation/pom.xml presentation/

COPY domain/src domain/src
COPY application/src application/src
COPY infrastructure/mysql/src infrastructure/mysql/src
COPY presentation/src presentation/src

RUN mvn clean package -DskipTests

FROM eclipse-temurin:21-jre
WORKDIR /app

COPY --from=build /app/presentation/target/user-api.jar app.jar

EXPOSE 8001

ENTRYPOINT ["java", "-jar", "app.jar"]