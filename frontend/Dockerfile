FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --configuration=production --base-href /contador-calorias/

FROM nginx:alpine AS final

COPY nginx.conf /etc/nginx/conf.d/default.conf

# Copia para dentro do subdiretorio contador-calorias
RUN mkdir -p /usr/share/nginx/html/contador-calorias
COPY --from=build /app/dist/frontend/browser /usr/share/nginx/html/contador-calorias

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]