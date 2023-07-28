package fruitProcessing;

import org.eclipse.microprofile.reactive.messaging.*;

import fruitProcessing.model.FruitMessage;

import javax.enterprise.context.ApplicationScoped;

@ApplicationScoped
public class ProcessFruitApplication {

    /**
     * Consume the message from the source channel, uppercase it and send it to the
     * uppercase channel
     * @throws InterruptedException
     **/
    @Incoming("fruit")
    @Outgoing("fruit-buy")
    public FruitMessage verifyBuyFruit(FruitMessage message) throws InterruptedException {
        System.out.println(">> Processing >> " + message.fruit.name + " " + message.fruit.quantity);

        if (message.fruit.quantity < 130) {
            System.out.println(">> Buy >>" + message.fruit.name + " " + message.fruit.quantity);
            message.fruit.quantity += 100;
            return message;

        }
        return null;
    }

    // /** Consume the uppercase channel and print the message **/
    // @Incoming("fruit-buy")
    // public void sink(FruitMessage fruit) {
    // System.out.println(">> buying " + fruit.fruit.name);
    // }
}
