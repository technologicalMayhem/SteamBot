namespace technologicalMayhem.SteamBot
{
    static class ChatMessages
    {   
        //Normal Messages
        public const string WelcomeMsg = "Hallo, ich bin der unausstehliche Bot. Ich kann mithilfe von Chatbefehle ein paar simple Aufgaben übernehmen.\nDaher du jedoch neu bist hast du nur einen sehr begrenzten Zugriff. Für eine Liste aller Befehle gebe einfach 'help' ein.";
        public const string UnknownCommandMsg = "Unbekannter Befehl. Schreibe 'help' um eine Liste aller vefügbaren Befehle zu erhalten.";
        public const string HelpBeginning = "Vefügbare Befehle:";
        public const string HelpEnding = "Eventuell sind einige Befehle nicht aufgeführt daher du nicht ausreichend Berechtigungen hast.\nMöchtest du mehr über die Funktion oder Nutzung eines bestimmten Befehls erfahren nutze einfach den Befehl 'help <name des Befehls>'";
        //Permission Change Messages
        //These Messages are used when a user gains or loses a rank or simply asks for his current rank.
        //They consist of a base me ssage and then a individual rank message.
        //Base Messages
        public const string RankIncreasedMsg = "Dein Rang wurde erhöht, du bist nun Rang ";
        public const string RankDecreasedMsg = "Dein Rang wurde gesenkt, du bist nun Rang ";
        public const string RankInfoMsg = "Dein aktueller Rank ist ";
        //Rank Messages
        public const string Rank0Msg = "0";
        public const string Rank1Msg = "1";
        public const string Rank2Msg = "2";
        public const string Rank3Msg = "3";
        public const string Rank4Msg = "4";
    }
}