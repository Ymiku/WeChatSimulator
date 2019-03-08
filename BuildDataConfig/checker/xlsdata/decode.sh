#!/usr/bin/env bash
cd $(dirname ${0})

DATABASE_NAME=${1}
if [ -z "${DATABASE_NAME}" ]
then
	DATABASE_NAME='champion_bits'
fi

printf "$(protoc --decode=${DATABASE_NAME} data.proto < data/${DATABASE_NAME}.data)"
printf "\n"