#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using OpenAI_API;
using OpenAI_API.Chat;
using OpenAI_API.Models;
using System;

public static class ChatGPT
{
    public static OpenAIAPI api;

    private static List<ChatMessage> messages;

    public static void StartConversation(string language)
    {
        /*
        messages = new List<ChatMessage> {
            new ChatMessage(ChatMessageRole.System, "("+ textEnglish + ")" + " Translate this to " +
            "Simple Chinese, " +
            "Traditional Chinese," +
            "Japanese," +
            "Korean," +
            "French," +
            "German," +
            "Spanish," +
            "Portuguese," +
            "Italian," +
            "Dutch," +
            "Polish," +
            "Turkish," +
            "Russian," +
            "Arabic," +
            "Thai," +
            "Vietnamese," +
            "Indonesian," +
            "Hindi," +
            "using '^' to separate them. kepp it in this order, reply me in translation text and '^', no other letter, no speace between '^' ")
        };*/

        messages = new List<ChatMessage>();

        messages = new List<ChatMessage> {
            //new ChatMessage(ChatMessageRole.System, "I will send you an English text, Translate it to " + language + " reply in translated text ONLY, no other text.") // textEnglish;
           new ChatMessage(ChatMessageRole.System, "I will give you an English text. translate it to " + language + ". Give me the translated text ONLY, DO NOT give me the english text. Do NOT repley any otehr letter.")
     };

        //textGPT = "";
        //string startString = "Waiting...";
        //textGPT = startString;
        //Debug.Log(startString);
    }

    public static async void GetResponse(string textEnglish, LanguageType language, Action<string> callbackSucceed)
    {
        // Fill the user message from the input field
        ChatMessage userMessage = new ChatMessage();
        userMessage.Role = ChatMessageRole.User;
        userMessage.Content = textEnglish;

        if (userMessage.Content.Length > 300)
        {
            // Limit messages to 100 characters
            userMessage.Content = userMessage.Content.Substring(0, 300);
        }
       // UnityEngine.Debug.Log(string.Format("{0}: {1}", userMessage.rawRole, userMessage.Content));

        // Add the message to the list
        messages.Add(userMessage);

        // Send the entire chat to OpenAI to get the next message
        var chatResult = await ChatGPT.api.Chat.CreateChatCompletionAsync(new ChatRequest()
        {
            Model = Model.ChatGPTTurbo,
            Temperature = 0.9,
            MaxTokens = 2000,
            Messages = messages
        });

        // Get the response message
        ChatMessage responseMessage = new ChatMessage();
        responseMessage.Role = chatResult.Choices[0].Message.Role;
        responseMessage.Content = chatResult.Choices[0].Message.Content;
       // UnityEngine.Debug.Log(string.Format("{0}: {1}", responseMessage.rawRole, responseMessage.Content));

        //resule = responseMessage.Content;

        callbackSucceed?.Invoke(responseMessage.Content);
    }
}

#endif
