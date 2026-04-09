FROM node:22-alpine AS build
WORKDIR /app

# Copia e instala dependências primeiro (otimiza o cache do Docker)
COPY package*.json ./
RUN npm install

# Copia o código e faz o build
COPY . .
RUN npm run build --configuration=production

# Estágio 2: Servidor com Nginx
FROM nginx:alpine AS final

# Copia a configuração do Nginx que criamos no passo 2
COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia os arquivos do build do Angular para a pasta pública do Nginx
# ATENÇÃO: Substitua "nome-do-projeto" pelo nome que está no seu angular.json
COPY --from=build /app/dist/nome-do-projeto/browser /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]