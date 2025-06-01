FROM node:20 AS builder

WORKDIR /app

COPY src/frontend/Cdb.Calculator.App/ .

RUN npm install
RUN npm run build

# Stage 2: Servir com Nginx
FROM nginx:alpine

COPY --from=builder /app/dist/CdbCalculator.Web/browser usr/share/nginx/html

COPY src/frontend/Cdb.Calculator.App/nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]