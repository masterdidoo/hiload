#FROM microsoft/aspnetcore:2.0
FROM example/aspnetcore-nginx

#ENV http_proxy=http://192.168.99.1:8080
#ENV https_proxy=http://192.168.99.1:8080

ENV http_proxy=
ENV https_proxy=

#RUN apt-get update
#RUN apt-get install -y nginx
 
WORKDIR /app
RUN rm -rf *
COPY ./out .
 
COPY ./startup.sh .
RUN chmod 755 /app/startup.sh
 
RUN rm /etc/nginx/nginx.conf
COPY nginx.conf /etc/nginx
 
ENV ASPNETCORE_URLS http://+:5000
EXPOSE 5000 80

RUN env
 
CMD ["sh", "/app/startup.sh"]