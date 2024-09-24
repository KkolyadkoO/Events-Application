# Используем Node.js для сборки React-приложения
FROM node:20 AS build
WORKDIR /app

# Устанавливаем зависимости
COPY package*.json ./
RUN npm install --legacy-peer-deps

# Копируем исходники и собираем приложение
COPY . ./
RUN npm run build

# Используем Nginx для обслуживания статических файлов React
FROM nginx:alpine
COPY --from=build /app/build /usr/share/nginx/html

# Открываем порт 80, так как Nginx обслуживает на этом порту
EXPOSE 80

# Запускаем Nginx
CMD ["nginx", "-g", "daemon off;"]
