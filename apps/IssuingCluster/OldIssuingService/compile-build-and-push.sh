#!/bin/bash
set -e

(cd .. && ./compile.sh)
./build-and-push.sh
