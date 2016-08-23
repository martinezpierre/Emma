Fonctionnement des tools (normalement ca devrait marcher aussi sur PC):

1) pour le texte :

Editez le fichier french.txt situé dans Assets/Resources/Texts ou remplacez le par un fichier du même nom pour changer le texte. Il est important de respecter la mise en forme en utilisant 
[SceneNumber]
<number>
avec number le numéro du dialogue et SceneNumber le numéro de la scène.
Vous pouvez également changer les traductions en éditant les autres fichiers présents dans le dossier Texts.

2) le Text Manager :

Cliquez en haut sur MyTools/Text Manager pour ouvrir le Text Manager. Cela va ouvrir une fenêtre dans laquelle sera listée tout les dialogues fournis dans french.txt.
(les deux boutons en haut sont des boutons de debug, ne les utilisez pas)
En cliquant sur le petit + à droite d’une phrase, une fenêtre d’édition va s’ouvrir.
Voici les options modifiables ici :
-Delay before appear : vous permet de laisser un temps avant que le texte ne commence à s’afficher.

-Character : vous permet de sélectionner la personne à l’origine de la phrase (pour le nom et la couleur du texte). Vous pourrez en rajouter dans le Character Manager (laissez le champs vide pour le narrateur).

-SMS : Cette option vous permet de choisir si le message sera un sms. sont affichage sera alors fait dans une bulle de la couleur du personnage. Une image peut être ajoutée à un sms, avant ou après le message.

-Change images : cochez cette case si vous voulez que cette phrase garde les mêmes images que la phrase précédente.
-Transition type : vous permet de choisir le mode de transition qu’utilisera cette image pour son apparition. Simple fait disparaitre l’image précédent et apparaitre cette image instantanément. Fade fait apparaitre l’image avec une fondue au noir et crossfade fait apparaitre l’image avec une fondue enchainée. Si vous choisissez fade ou crossfade vous pourrez alors choisir leur durée.
-Edit anim : cochez cette case pour faire apparaitre les informations d’animations. vous pouvez ici paramétrer la position de départ de l’image, sa rotation et sa scale, ainsi que les mêmes informations au terme de l’animation et la durée de celle-ci. Si vous voulez juste changer l’endroit ou apparait une image, sa taille ou sa rotation, changez les deux cotés.
cocher le bouton Ease permet de lisser la courbe d’animation avec une accélération et une décélération.
-Appear after sentence : cochez cette case si vous voulez que l’image apparaisse après la fin de la phrase et nécessite un clic pour passer à la suite (comme les sms par exemple)
-new image: rajoute une image à la liste. l’image la plus en haut apparaitra le plus au font.
-new game object: rajoute un game object. comme pour l’image, il suffi ensuite de drag and drop le prefab voulu dans le champs (à utiliser pour les fx).

-Theme: vous permet de sélectionner la musique principale du niveau. Laissez le champ vide pour garder la musique actuelle. en cochant Options, vous pourrez paramétrer la durée de son fade In, de son fade Out et le delay avant le début de la musique.
-Sounds: vous permet d’ajouter un son qui se jouera au moment d’apparition du texte. En plus des options du theme, vous pouvez cochez Loop pour faire boucler le son (le theme boucle automatiquement)

-new Choice permet d’ajouter une possibilité de choix après la phrase (4 choix max). Remplissez le champs le plus à gauche avec le texte que vous voulez voir sur le bouton de choix. Pour paramétrer l’ensemble de phrase vers lequel le choix va emmener remplissez les deux autres champs avec l’index de la première phrase de la sequence et celui de la dernière en utilisant la mise en forme du TextEditor (ex: 3.4). Les phrases correspondantes seront affichés si les index sont correctes.
Les embranchements correspondants seront affichés dans le TextEditor.

-Save and Close vous permet de sauvegarder vos modifications et de fermer la fenêtre. (ATTENTION ! Si vous fermez la fenêtre autrement toute les modifications effectuez seront perdues)

3) le Character Manager :

Cliquez en haut sur MyTools/Character Manager pour ouvrir le Character Manager. Cliquez ensuite sur New Character pour rajouter un personage. Vous pouvez ensuite modifier son nom et sa couleur dans les champs qui apparaitront.
N’oubliez pas de cliquez sur Save pour sauvegarder vos modifications.

4) le Translation Editor :

Cliquez sur MyTools/Translation Editor afin d’ouvrir le Translation Editor. cet éditeur vous permet de traduire les textes présents dans l’UI en plusieurs langues (je me suis basé sur les langues mentionnées dans le pdf présentant le jeu, si vous voulez d’autres langues, demandez moi).
A l’ouverture, le translation editor va directement récupérer dans la scène actuelle tout les élément d’UI textuel. Remplissez juste les champs pour que chaque texte ai la traduction adéquate.
Comme d’habitude, n’oubliez pas de sauvegarder avant de quitter.

5) le custom build :

Afin de faire fonctionner le build du jeu (au moins sur mac), vous devez passer par MyTools/Custom Build. Sélectionnez ensuite juste le dossier dans lequel vous voulez créer le build et le jeux se lancera automatiquement après le build.

6) Options :

Plusieurs options sont déjà accessible sur le game object Options présent dans la scene. Si vous avez besoin de plus d’options, demandez.

7) notes :

Le menu est actuellement en cours de développement. Les fonctions Commencer et Reprendre devraient fonctionner mais les modifications de langues ne seront pas conservée.
Mes excuses d’avance pour l’UI par défaut à certains endroit, c’est juste là pour le test.
Désolé aussi pour la justification de texte qui n’a pas encore été rajoutée.

De nombreux champs ont étés remplis pour le test et n’ont donc aucun sens en jeu. Je m’excuse par avance pour la traduction google trad utilisée afin de tester la feature de traduction.

La position du joueur dans l’histoire est représentée en jeu par le slider présent dans le menu de pause.

