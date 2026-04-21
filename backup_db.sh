#!/bin/bash

# Configurações
BACKUP_DIR="/root/backups_db"
DB_CONTAINER="contador-calorias-db"
DB_USER="usuario"
DB_NAME="CalculadoraCaloriasDb"
DATE=$(date +%Y-%m-%d_%H-%M-%S)

# Criar pasta de backup se não existir
mkdir -p $BACKUP_DIR

# Gerar o backup
docker exec -e PGPASSWORD='Senha123@' $DB_CONTAINER pg_dump -U $DB_USER $DB_NAME > $BACKUP_DIR/db_backup_$DATE.sql

# Opcional: Remover backups com mais de 7 dias para não encher o disco
find $BACKUP_DIR -type f -name "*.sql" -mtime +7 -delete

echo "Backup realizado com sucesso em $DATE"