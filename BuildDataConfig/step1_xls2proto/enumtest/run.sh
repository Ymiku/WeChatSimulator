#!/usr/bin/env bash

cd $(dirname ${0})/..

protoc --python_out=. ProtoFScalar.proto
python xls_deploy_tool.py REWARD_CONF enumtest/reward.xlsx
protoc --decode=dataconfig.REWARD_CONF_ARRAY *.proto < dataconfig_reward_conf.data

rm -f dataconfig_*.* *_pb2.*