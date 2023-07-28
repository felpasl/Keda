package fruitProcessing.serializer;

import fruitProcessing.model.FruitMessage;
import io.quarkus.kafka.client.serialization.JsonbDeserializer;

public class FruitDeserializer extends JsonbDeserializer<FruitMessage>{
    public FruitDeserializer(){
        super(FruitMessage.class);
    }
}
