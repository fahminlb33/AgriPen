# build stage
FROM node:18-alpine AS builder

WORKDIR /app

COPY [".env", "package.json", "package-lock.json", "./"]

RUN npm ci

COPY . .

RUN npm run build

# runtime stage
FROM nginx:alpine AS runtime

WORKDIR /usr/share/nginx/html

COPY --from=builder /app/dist ./

COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 4000
