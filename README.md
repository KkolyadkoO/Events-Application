# Тестовое задание

## Инструкция по запуску

1. ```git clone https://github.com/KkolyadkoO/Events-Application.git```
2. Потом необходимо собрать frontend, backend и базу
   * Находясь в клонированной папке в которой находится docker-compose.yml (```cd Events-Application```)
   * ```docker-compose up --build```
3. После успешной сборки появится контейнер в котором находятся
   * backend 
   * frontend
   * database
4. Для открытия swagger переходим по пути http://localhost:5000/swagger/index.html
5. Для открытия сайта переходим по пути http://localhost:3000/

В базе уже есть пользователь с ролью администратора 
   * login: admin
   * password: admin