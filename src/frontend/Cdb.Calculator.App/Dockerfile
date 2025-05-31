# Dockerfile for Frontend Web App (CdbCalculator.Web)

# Stage 1: Build the Angular application
FROM node:20 AS build
WORKDIR /app

# Copy package.json and package-lock.json (if available)
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application for production
RUN npm run build -- --configuration production --base-href / --deploy-url /

# Stage 2: Serve the application using Nginx
FROM nginx:alpine

# Copy the built application from the build stage to Nginx's web root directory
COPY --from=build /app/dist/CdbCalculator.Web /usr/share/nginx/html

# Copy custom Nginx config
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Expose port 80 for Nginx
EXPOSE 80

# Start Nginx when the container launches
CMD ["nginx", "-g", "daemon off;"]
