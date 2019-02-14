# coding=utf-8

# This functions analyze the given text for language, sentiments, keyphrases and entities 
# with REST-API for Text Analyzer of the Cognitive Services

import requests
from pprint import pprint

# Experiment 1. - analyse language
def Experiment_1(headers, base_url):
    pprint("Experiment 1. - analyse language")
    pprint("--------------------------------")
    analyze_url = base_url + "languages"
    documents = { 'documents': [
        { 'id': '1', 'text': 'Das ist ein Text zum Testen. Sie können hier auch ihren eigenen Text in verschiedenen Sprachen hinzufügen.' },
        { 'id': '2', 'text': 'This is a text for testing. You can also add your own text in different languages here.' },
        { 'id': '3', 'text': 'Ceci est un texte à tester. Vous pouvez également ajouter votre propre texte dans différentes langues ici.' },
        { 'id': '4', 'text': '這是一個測試文本。 您也可以在此處添加不同語言的文本。' }
    ]}
    # query Text service on Azure 
    response  = requests.post(analyze_url, headers=headers, json=documents)
    languages = response.json()

    pprint("INPUT:")
    pprint(documents)
    pprint("")
    pprint("OUTPUT:")
    pprint(languages)
    pprint("")

# Experiment 2. - analyse sentiments 
def Experiment_2(headers, base_url):
    pprint("Experiment 2. - analyse sentiments")
    pprint("--------------------------------")
    analyze_url = base_url + "sentiment"
    documents = {'documents' : [
      {'id': '1', 'language': 'en', 'text': 'I had a wonderful experience! The rooms were wonderful and the staff was helpful.'},
      {'id': '2', 'language': 'en', 'text': 'I had a terrible time at the hotel. The staff was rude and the food was awful.'}
    ]}
    # query Text service on Azure 
    response  = requests.post(analyze_url, headers=headers, json=documents)
    sentiments = response.json()

    pprint("INPUT:")
    pprint(documents)
    pprint("")
    pprint("OUTPUT:")
    pprint(sentiments)
    pprint("")
 
# Experiment 3. - extract keyphrases
def Experiment_3(headers, base_url):
    pprint("Experiment 3. - extract keyphrases")
    pprint("--------------------------------")
    analyze_url = base_url + "keyPhrases"
    documents = {'documents' : [
      {'id': '1', 'language': 'ja', 'text': '猫は幸せ'},
      {'id': '2', 'language': 'de', 'text': 'Fahrt nach Stuttgart und dann zum Hotel zu Fuß.'},
      {'id': '3', 'language': 'en', 'text': 'My cat is stiff as a rock.'},
      {'id': '4', 'language': 'es', 'text': 'A mi me encanta el fútbol!'}
    ]}
    # query Text service on Azure 
    response  = requests.post(analyze_url, headers=headers, json=documents)
    key_phrases = response.json()

    pprint("INPUT:")
    pprint(documents)
    pprint("")
    pprint("OUTPUT:")
    pprint(key_phrases)
    pprint("")

# Experiment 4. - entities identification
def Experiment_4(headers, base_url):
    pprint("Experiment 4. - entities identification")
    pprint("--------------------------------")
    analyze_url = base_url + "entities"
    documents = { 'documents': [
        { 'id': '1', 'text': 'Erstellen Sie ein kostenloses Azure-Konto, um kostenlos auf Cognitive Services zuzugreifen, oder erfahren Sie mehr über die Kaufoptionen für den Produktionseinsatz.' },
        { 'id': '2', 'text': 'Erfahren Sie mehr über Sprachdienste, die sicherstellen, dass Apps und Dienste die Bedeutung von unstrukturiertem Text verstehen oder die Absicht hinter den Äußerungen eines Sprechers erkennen können. Testen Sie Dienste kostenlos, und erstellen Sie im Handumdrehen sprachbasierte Apps und Dienste mit den folgenden Funktionen.' }
    ]}
    # query Text service on Azure 
    response  = requests.post(analyze_url, headers=headers, json=documents)
    entities = response.json()

    pprint("INPUT:")
    pprint(documents)
    pprint("")
    pprint("OUTPUT:")
    pprint(entities)
    pprint("")

