INCLUDE global.ink

~ character_index = 2

{ adelina_dialog_index:
- 0: -> AdelinaDialog_0_0_0
- 1: -> AdelinaDialog_1_0_0
}

===AdelinaDialog_0_0_0===
Greetings, newbie!
     * [Well hello.]
         What a cold greeting.
         -> AdelinaDialog_0_0_1
     * [Hello. Are you one of the Gardeners too?]
         Well, not really.
         Let's say, I'm here a bit in the role of a guest.
         -> AdelinaDialog_0_0_1
     * [What did such a fragile and beautiful girl forget in this terrible forest?]
         You say one more time about a fragile girl, and this wrench will fly between your eyes.
         -> AdelinaDialog_0_0_1
===AdelinaDialog_0_0_1===
My name is Adelina.
I am a local seller. Selling items that help Gardeners survive the hellish battle with monsters.
     * [{ ernest_dialog_index == 2:
Wait, isn't Ernest supposed to supply me with gear and other survival stuff?
         }]
         Well... maybe so, but I also have a couple of interesting things to sell.
         ~character_index = 1
         And if I can sell them, then why not sell them to you, in order to save life!
         ~character_index = 2
         I sometimes give her my inventions so that she can take them away from me. And then she sells them and makes money off simpletons like you.
         ~character_index = 1
         Ernest, you know that eavesdropping on other people's conversations is not polite.
         ~character_index = 2
         In general, I am not paid here for etiquette and a normal attitude towards others. That's why I somehow sneeze.
         Ah... ignore him, he's just a little out of sorts.
         -> AdelinaDialog_0_0_2
     * [How did an ordinary saleswoman get into the area where monsters live?]
         Professional salespeople always have their own ways of reaching their potential clients!
         Maybe if one day you want to sell goods, you also know these ways.
         -> AdelinaDialog_0_0_2
     * [Damn capitalists! Even in times of danger to humanity, you live to make money!]
         You see, it's better for us to make money now, so that we don't have to do it in post-apocalypse conditions later, and we don't sell goods for cola caps.
         -> AdelinaDialog_0_0_2
===AdelinaDialog_0_0_2===
~ adelina_dialog_index = 1
By the way, my store does not accept money!
Still, I understand that you cannot get them in the forest.
So if you want to buy something from my shop, bring me materials that can be obtained from monsters.
You have no idea how valuable they are on the black market!
So, if you have extra materials, I'm always happy to talk with you, and even better, sell something!
-> END


===AdelinaDialog_1_0_0===
~ temp welcome_phrase = RANDOM(1,4)
{welcome_phrase:
- 1: Greetings!
- 2: Do you want to buy something?
- 3: I'm glad you're still alive!
- 4: Long time no see!
}

     * [Anything for sale?]
         ~ open_shop = true
         Of course!
         ->END
     * [Good bye!]
         Bye bye!
         ->END