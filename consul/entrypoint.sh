#!/bin/sh

set -e

_dirname=$(
    cd "$(dirname "$0")"
    pwd -P
)

consul agent -dev -client=0.0.0.0 &
echo "Consul agent started."

until curl "http://localhost:8500/v1/status/leader"; do
    echo "Waiting for Consul to be ready..."
    sleep 1
done

for jsonPath in $(ls $_dirname/kv/*); do
    key=$(basename $jsonPath .json)
    value=$(cat $jsonPath)
    curl --silent --request PUT --data "$value" http://localhost:8500/v1/kv/$key
done
echo "Completed inserting key/value pairs."

tail -f /dev/null
