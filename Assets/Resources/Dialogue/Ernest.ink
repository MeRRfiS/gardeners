INCLUDE global.ink

~ open_error = 0
~ character_index = 1

{ernest_dialog_index:
- 0: -> ErnestDialog_0_0_0
- 1: -> ErnestDialog_0_0_1
- 2: -> ErnestDialog_1_0_0
}

===ErnestDialog_0_0_0===
~ is_first_meet_with_ernest = false
I do not advise you to go there now without first talking to me.
Well, of course, if you want to die as soon as possible, then the flag is in your hands!
-> END
===ErnestDialog_0_0_1===
~ ernest_dialog_index = 1
{ is_first_meet_with_ernest: Oh! You are already awake. | The right choice. }
     * [I'm guessing you're kind of like my monster pain partner?]
         Well... You pretty much guessed it, except for the part where you think of me as your comrade in arms.
         ->ErnestDialog_0_0_2
     * [What the hell are you?]
         Usually after such words at Hogwarts, 100 faculty points are deducted.
         But unfortunately, we are not in the magical world of magic and magic potions, so I'll pretend I didn't hear you.
         ->ErnestDialog_0_0_2
    
===ErnestDialog_0_0_2===
My name is Ernest, and I am the Gardener responsible for outreach, research and technical support.
I am something like a local God here.
     * [It seems to me that you are exaggerating.]
         I agree, maybe I exaggerated a little.
         I still lack God-level immortality, but it's in the process.
         ->ErnestDialog_0_0_3
     * [So it turns out that you are the most useless here.]
         Usually, after such words, the next day, our office holds funerals for new team members.
         And it happens so often that I even forgot the last time I took off this black suit.
         ->ErnestDialog_0_0_3
        
===ErnestDialog_0_0_3===
So, welcome to the ranks of Gardeners!
Admittedly, it's a bit of a strange name for someone who kills monsters in packs rather than growing apple trees in their backyard, but I have a logical explanation for it.
See that big tree?
Well, it's not a baobab, but a giant cursed thing that creates many monsters who only dream of a dinner of human flesh, a bloody Mary cocktail, and ice cream for dessert.
And our task is to destroy this cursed tree by destroying its root system.
That is why the Gardeners, because we are like uprooting a tree here, like real gardeners...
Okay, stupid name, and even here it is not saved by a logical explanation.
     * [Thanks for the explanation.]
         Do not mention it. I am waiting for 20 hryvnias on this table tomorrow.
         -> ErnestDialog_0_0_4
     * [Wow Einstein, thanks for reminding me, I read about it in the handbook while I was driving.]
         Oho! You can even read! The 8th wonder of the world is coming out!
         -> ErnestDialog_0_0_4
        
===ErnestDialog_0_0_4===
So, before you go to defend the world from a horde of monsters, grab this.
[received by PVST]
~ get_KVZHP = 1
     * [What it is?]
         -> ErnestDialog_0_0_5
     * [A cool accessory.]
         Congratulations! Now you have at least one thing that makes you more attractive in this God-filled place!
         -> ErnestDialog_0_0_5
        
===ErnestDialog_0_0_5===
This is a "Pocket Vital Indicators Tracker" - a device that will allow you to monitor your indicators during the battle, and also in a difficult situation, it can return to this place.
If it's easier, he will save you from meeting dead relatives.
     * [Cool! Now I can live a little longer.]
         Just be careful with this thing!
         If anything happens to her, I will shake the last penny out of you!
         -> ErnestDialog_0_0_6
     * [Does it work for sure?]
         Of course! This thing passed all possible tests!
         True, this is the first time we will test it in the conditions of a real battle...
         In any case, if even something goes wrong, you won't even guess about it.
         After all, the dead do not speak.
         -> ErnestDialog_0_0_6
     * [What if I don't want to wear it?]
         Well, then don't be surprised in the world why it will be written on your grave: "The biggest fool in the world is buried here."
         -> ErnestDialog_0_0_6
        
===ErnestDialog_0_0_6===
~ ernest_dialog_index = 2
That's all!
If you have any questions, you can ask me at any time!
Although I am not lying, from Monday to Friday from 9 to 18, on Saturday to 17, I am ready to answer your questions.
I have a day off on Sunday, so if you come to me with a question, I'll set a swarm of wild bees on you!
     * [Umm... ok. See you then.]
         Yeah, all the best.
         -> END
     * [You don't have bees.]
         Even if there isn't, do you think I won't do something worse to you than be stung by bees?
         Ok... I'll see you later, I've got some work to do.
         -> END

===ErnestDialog_1_0_0===
~ temp welcome_phrase = RANDOM(1,4)
VAR start_dialog = true
{start_dialog:
{welcome_phrase:
- 1: Any questions?
- 2: Long time no see.
- 3: Look, he is not dead yet.
- 4: I am listening.
}
- else: Another question?
}
~ start_dialog = false

     * [How are you?]
         It was better until you came.
         -> ErnestDialog_1_0_0
     * [{ first_game == 1:
Wow, I'm still alive. I thought I was going to die...
         }]
         So, my "Pocket Vital Signs Tracker" still worked.
         Although there is some good news for the last month of work.
         Wait, I've only been working for a month.
         Well... that doesn't change the above.
         ~ first_game = 2
         -> ErnestDialog_1_0_0
     * [{ what_i_must_do == 1:
I didn't understand something, what I should do...
     }]
         Are you serious now?
         Ah... why is it so difficult for you?...
         So, listen carefully: have you seen the roots and trees from which monsters come out?
         So, they are some key to the main root. If you destroy them, a passage should appear.
         This is, so to speak, one of the ways to protect that big tree.
         ** [Wait, protection methods? How can a tree defend itself?]
             It still remains a mystery.
             We have not yet studied this forest and its local monsters enough to answer this question with certainty.
             One of the theories is that the tree, in order not to be able to destroy its roots, acquired these methods in the process of evolution.
             However, I personally do not really believe in this theory.
             It is unlikely that there are organisms that can evolve so quickly.
             If I had materials from the forest, I could investigate this phenomenon.
             But due to the fact that I do not have them, it will rather remain a mystery.
             ~ what_be_after_destroy = 1
             ~ what_i_must_do = 2
             -> ErnestDialog_1_0_0
         ** [Well, this probably makes sense.]
             ~ what_be_after_destroy = 1
             ~ what_i_must_do = 2
             -> ErnestDialog_1_0_0
     * [{genereted_map == 1:
     The second time I entered the forest, it had changed. It feels like I entered a completely different location.
     }]
         Great, we were sent a person with topographical cretinism.
         OK, I'm kidding.
         Yes, there is such a thing. Every time the forest changes its shape, after a person leaves it.
         It is difficult to explain why this happens, it is another unsolved mystery.
         Maybe another defense mechanism, or maybe the ants burrow into the ground, thus changing the forest.
         Who knows?..
         ~ genereted_map = 2
         -> ErnestDialog_1_0_0
     * [{what_be_after_destroy == 1:
     What happens after I destroy the roots?
     }]
         ~ character_index = 3
         Well, look...
         ~ open_error = 1
         SYSTEM: ERROR - TRYING TO ACCESS MISSING CODE
         SYSTEM: ERROR STATUS IS NOT CRITICAL
         SYSTEM: ACTION TO RESOLVE ERROR - REMOVE METHOD REFERENCED BY MISSING CODE!
         SYSTEM: TRYING TO RESUME SESSION - 1...
         SYSTEM: TRYING TO RESUME SESSION - 2...
         ~ character_index = 1
         SYSTEM: TRYING TO RESUME SESSION - 3...
         ~ open_error = 0
         ~ what_be_after_destroy = 2
         Hey what's wrong with you
         You look as if you have just seen death itself.
         Although, you do it almost every day anyway.
         You'd better go and rest, otherwise you'll kill someone from the bad feeling.
         -> END
     * [No, nothing.]
         Fine.
         -> END