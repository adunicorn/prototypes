set -e

echo Compiling the new IssuingService
(cd IssuingService && ./compile.sh)
(cd OldIssuingService && source compile.sh)
(cd RabbitConsumer && source compile.sh)
