using Newtonsoft.Json;
using Quobject.SocketIoClientDotNet.Client;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Fluxx
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GamePage : ContentPage
    {
        enum Orientation { left, top, right, bottom }
        public event EventHandler AnimationCompleted = delegate { };
        Card[] Cards;
        CardView[] CardsViews;
        RelativeLayout card;
        Socket socket;
        Room _room;
        Player _player;
        public bool test;

        public GamePage(Room room, Player player)
		{
            InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            _room = room;
            _player = player;
            socket = new ServerConnection().Socket();
            ChatBox chat = new ChatBox(_player, _room);
            Chat.Children.Add(chat);
            Cards = LoadCard();
            CardsViews = new CardView[Cards.Length];
            int nbrPersonne = 3;// room.Players.Length;
            SetBoard(nbrPersonne);
            test = true;
		}
        private void SetBoard(int nbrPersonnes)
        {
            for (int i = 0; i < Cards.Count()-4; i=i+3)
            {
                Start();
                card = CardView(Cards[i].Id, ((Application.Current.MainPage.Height-Chat.Height)*25/100));
                Test.Children.Add(card);//, Constraint.Constant(0), Constraint.Constant(0), Constraint.Constant(46 * 7), Constraint.Constant(46 * 11)

            }

            switch (nbrPersonnes)
             {
                 case 2:

                     break;
                 case 3:
                         break;
                 case 4:

                     break;
                 case 5:

                     break;
                 case 6:

                     break;

             }
        }
        private void Start()
        {

        }
       
      /*  private void AnimCardDeplacement(CardView card, int xfin, int yfin)
        {
            Animation a = new Animation();
            //initial.X
        }*/
        private StackLayout CreateHand(Orientation orientation, int playerID, bool my)
        {
            if (my)
                return new StackLayout {
                    Padding = new Thickness(15, 0, 15, 0),
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Children = { }
                };

            else
                return new StackLayout {
                    Padding = new Thickness(15, 0, 15, 0),
                    Orientation = StackOrientation.Horizontal,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.StartAndExpand,
                    Children = {  }
                }; 

        }
        private StackLayout NewCarList(string name)
        {
            return new StackLayout
            {
                Padding = new Thickness(15, 0, 15, 0),
                Orientation = StackOrientation.Horizontal,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Children = { }
            };
        }
        private RelativeLayout CardView(int cardId, double cardHeight)
        {
            float horizontalFormat = 7;
            float verticalFormat = 11;
            RelativeLayout result = new RelativeLayout();
      
            
               /* Image backCardImg = new Image { Source = "back_card.png" };

                result.Children.Add(backCardImg,
                    Constraint.Constant(0),
                    Constraint.Constant(0),
                    Constraint.RelativeToParent((parent) => { return parent.Width; }),
                    Constraint.RelativeToParent((parent) => { return parent.Height; }));*/
            if (cardId == 0)
            {
                //firstcard
            }
            else if (cardId < Cards.Count())
            {                
                CardsViews[cardId] = new CardView(Cards[cardId], cardHeight, AnimLayout);
                result.Children.Add(CardsViews[cardId], Constraint.Constant(0), Constraint.Constant(0), Constraint.RelativeToParent((parent) => { return (result.Height/verticalFormat)*horizontalFormat; }), Constraint.RelativeToParent((parent) => { return result.Height; }));
            }
            else
            {

            }
            return result;
        }
        private void OnClick(object sender, EventArgs e)
        {
           
        }
        private Card[] LoadCard()
        {
            //todo correction par apport aux images
            string json = "[{\"id\"  :1,\"title\":\"On va Simplifier\",\"type\" :\"Action\",\"description\":\"Comptez les cartes Nouvelle Règle en jeu, puis défaussez-en la moitié (arrondir à l'entier supérieur) en les choissant.\"},{\"id\"  :2,\"title\":\"Duel de Pierre-Papier-Ciseaux\",\"type\" :\"Action\",\"description\":\"Défiez un autre joueur, qui doit disputer avec vous une partie de Pierre-Papier-Ciseaux en 3 rounds.\nLe vainqueur remporte toutes les cartes de la main du vaincu.\"},{\"id\"  :3,\"title\":\"Changement de Mains\",\"type\" :\"Action\",\"description\":\"Chaque joueur transmet sa main à son voisin\nVous choissisez dans quel sens se fait le transfert.\"},{\"id\"  :4,\"title\":\"Un Atout à la poubelle\",\"type\" :\"Action\",\"description\":\"Prenez un Atout posé devant n'importe quel joueur et mettez-le dans la Défausse.\nSi personne n'a d'Atout en jeu, il ne se passe rien quand vous jouez cette carte.\"},{\"id\"  :5,\"title\":\"Partage des Richesses\",\"type\" :\"Action\",\"description\":\"Prenez tous les Atouts de la table. Mélangez-les, puis distribuez-les en commançant par vous donner le premier.\nChacun se retrouvera probablement avec un nombre d'Atouts en jeu différent de ce qu'il avait à l'origine.\"},{\"id\"  :6,\"title\":\"Un Tour de Plus\",\"type\" :\"Action\",\"description\":\"Effectuez un autre tour de jeu dès que vous terminez celui en cours.\nCette carte ne permet pas d'effectuer plus de deux tours d'affilée.\"},{\"id\"  :7,\"title\":\"Videz la poubelle\",\"type\" :\"Action\",\"description\":\"Créez une nouvelle défausse en commançant par cette carte, puis prenez les cartes de l'ancienne défausse et mélangez-les à la Pioche.\"},{\"id\"  :8,\"title\":\"Jackpot !\",\"type\" :\"Action\",\"description\":\"Piochez 3 cartes supplémentaires !\"},{\"id\"  :9,\"title\":\"Raffler une carte\",\"type\" :\"Action\",\"description\":\"Choissisez n'importe quelle carte en jeu, n'importe où sur la table (àlexception des Règle de Base) et mettez-la dans votre main.\"},{\"id\"  :10,\"title\":\"Nouvelle Règle à la poubelle\",\"type\" :\"Action\",\"description\":\"Choissisez l'une des cartes Nouvelle Règle en jeu et mettez-la dans la Défausse.\"},{\"id\"  :11,\"title\":\"Utiliser ce que vous Prenez\",\"type\" :\"Action\",\"description\":\"Prenez une carte au hazard dans la main d'un autre joueur et jouez-la aussitôt.\"},{\"id\"  :12,\"title\":\"Pas de Limites\",\"type\" :\"Action\",\"description\":\"Défaussez toutes les règles concernant les Limites de Main et les Limites d'Atouts actuellement en jeu.\"},{\"id\"  :13,\"title\":\"Défaussez et Piochez\",\"type\" :\"Action\",\"description\":\"Défaussez-vous de toutes les cartes de votre main et piochez-en un nombre équivalent.\nCette carte ne compte pas pour ce qui est de déterminer le nombre de cartes à piocher.\"},{\"id\"  :14,\"title\":\"Piochez 2 et jouez-les\",\"type\" :\"Action\",\"description\":\"Laissez votre main de côté.\nPiochez 2 cartes, jouez-les dans l'ordre de votre choix, puis reprenez votre main et votre tour de jeu.\nCette carte et toutes les cartes qui en découlent comptent pour un seul carte.\"},{\"id\"  :15,\"title\":\"1 pour Tous\",\"type\" :\"Action\",\"description\":\" Laissez votre main de côté.\nComptez le nombre de joueur à la table (vous y compris). Piochez autant de cartes que de joueurs et distribuez 1 carte à chacun.\nVous décidez qui reçoit quoi.\"},{\"id\"  :16,\"title\":\"Échange d'Atouts\",\"type\" :\"Action\",\"description\":\"Choisisez un Atout qu'un autre joueur a posé sur la table et échangez-le-lui contre un Atout que vous avez posé.\nSi vous n'avez aucun Atout en jeu ou si personne d'autre n'a d'Atout sur la table, il ne se passe rien.\"},{\"id\"  :17,\"title\":\"Règles Réinitialisées\",\"type\" :\"Action\",\"description\":\"On reprend les Règles de Base.\nDéfaussez toutes les cartes Nouvelle Règle pour ne laisser que la carte Règles de Base en jeu.\nNe défaussez pas l'Objectif actuel.\"},{\"id\"  :18,\"title\":\"Journée Exceptionnelle\",\"type\" :\"Action\",\"description\":\"Laissez votre main de côté et piochez 3 carte. Si c'est votre anniversaire, jouez les 3 cartes. Si c'est un jour férié ou une date commémorative d'un genre ou d'un autre, jouez 2 des cartes. Si c'est un jour comme un autre, ne jouez que 1 carte. Défaussez le reste\"},{\"id\"  :19,\"title\":\"Piochez 3 et Jouez-en 2\",\"type\" :\"Action\",\"description\":\"Laissez votre main de côté.\nPiochez 3 cartes et jouez-en 2. Défaussez-vous de la carte restante, puis reprenez votre main et votre tour de jeu.\nCette carte et toutes les cartes qui en découlent comptent pour un seul carte.\"},{\"id\"  :20,\"title\":\"On la Refait !\",\"type\" :\"Action\",\"description\":\"Examiner la Défausse. Prenez-y la carte Action ou nouvelle Règle de votre choix et jouez-la aussitôt.\nTout le monde a le droit d'examiner le contenu de la Défausse quand il le souhaite, à condition de ne jamais changer l'ordre des cartes.\"},{\"id\"  :21,\"title\":\"Taxe aléatoire\",\"type\" :\"Action\",\"description\":\"Prenez 1 carte au hasard dans la main de chacun des autres joueurs. Les cartes obtenues sont placées dans votre main.\"},{\"id\"  :22,\"title\":\"Échange de Mains\",\"type\" :\"Action\",\"description\":\"Échangez votre main avec celle de l'un de vos adversaires.\nPour une fois que vous pouvez vous enrichir sans rien débourser !\"},{\"id\"  :23,\"title\":\"Vol d'Atout\",\"type\" :\"Action\",\"description\":\"Choissisez un Atout posé devant un autre joueur et ajoutez-le à votre propre collection d'Atouts sur la table.\"},{\"id\"  :24,\"title\":\"Dépêche !\",\"subtitle\":\"Action gratuite\",\"type\" :\"NewRule\",\"description\":\"Avant de jouer la dernière carte de votre tour, et si votre main n'est pas vide, vous pouvez à tout moment la défausser en entier et piocher 3 cartes. Votre tour s'achève alors immédiatement.\",\"image\":\"new_rule_despatch\"},{\"id\"  :25,\"title\":\"Piochez 2\",\"subtitle\":\"Remplace la Règle de Pioche\",\"type\" :\"NewRule\",\"image\":\"new_rule_drow2\",\"description\":\"Si vous venez de jouer cette carte et n'avez pas pioché au moins 2 cartes à ce tour, piochez-en asserz pour corriger la situation.\"},{\"id\"  :26,\"title\":\"Piochez 3\",\"subtitle\":\"Remplace la Règle de Pioche\",\"type\" :\"NewRule\",\"image\":\"new_rule_drow3\",\"description\":\"Si vous venez de jouer cette carte et n'avez pas pioché au moins 3 cartes à ce tour, piochez-en asserz pour corriger la situation.\"},{\"id\"  :27,\"title\":\"Piochez 4\",\"subtitle\":\"Remplace la Règle de Pioche\",\"type\" :\"NewRule\",\"image\":\"new_rule_drow4\",\"description\":\"Si vous venez de jouer cette carte et n'avez pas pioché au moins 4 cartes à ce tour, piochez-en asserz pour corriger la situation.\"},{\"id\"  :28,\"title\":\"Piochez 5\",\"subtitle\":\"Remplace la Règle de Pioche\",\"type\" :\"NewRule\",\"image\":\"new_rule_drow5\",\"description\":\"Si vous venez de jouer cette carte et n'avez pas pioché au moins 5 cartes à ce tour, piochez-en asserz pour corriger la situation.\"},{\"id\"  :29,\"title\":\"Bonus du bredouille\",\"subtitle\":\"Événement de début de tour\",\"type\" :\"NewRule\",\"image\":\"new_rule_empty_handed_bonus\",\"description\":\"Si vous n'avez plus de carte dans votre main, piochez 1 cartes avant de vous soumettre à la règle Piochez en vigeur.\"},{\"id\"  :30,\"title\":\"Échange d'objectifs\",\"subtitle\":\"Action gratuite\",\"type\" :\"NewRule\",\"image\":\"new_rule_exchange_goal\",\"description\":\"Une fois durant votre tour, défaussez autant de vos cartes Objectif que vous le voulez, et piochez un nombre équivalent de cartes.\"},{\"id\"  :31,\"title\":\"Échanger Jouer/Piocher\",\"subtitle\":\"Effet instantané\",\"type\" :\"NewRule\",\"image\":\"new_rule_exchange_play_drow\",\"description\":\"Durant votre tour, vous pouvez décider de ne plus jouer de cartes : à la place, vous piochez autant de cartes que vous aviez encore le droit de jouer. Si Jouez Tout est en jeu, piochez autant de cartes que celles en main.\"},{\"id\"  :32,\"title\":\"Bonus de Fête\",\"subtitle\":\"Effet instantané\",\"type\" :\"NewRule\",\"image\":\"new_rule_party_bonus\",\"description\":\"Si quelqu'un a La Fête en jeu, tous les joueurs piochent et jouent $number carte$s supplémentaire durant leur tour.\"},{\"id\"  :33,\"title\":\"Limite de main 0\",\"subtitle\":\"Remplace la Limite de Main\",\"type\" :\"NewRule\",\"image\":\"new_rule_card_limit0\",\"description\":\"Si ce n'est pas votre tour de jeu, votre main doit compter 0 carte : déffaussez-vous de toute carte en excès. Durant votre tour, cette règle ne s'applique pas à vous ; une fois votre tour terminé, défaussez autant de cartes que nécessaire pour une main de 0 carte.\"},{\"id\"  :34,\"title\":\"Limite de main 1\",\"subtitle\":\"Remplace la Limite de Main\",\"type\" :\"NewRule\",\"image\":\"new_rule_card_limit1\",\"description\":\"Si ce n'est pas votre tour de jeu, votre main ne doit compter que 1 carte : déffaussez-vous de toute carte en excès. Durant votre tour, cette règle ne s'applique pas à vous ; une fois votre tour terminé, défaussez autant de cartes que nécessaire pour une main de 1 carte.\"},{\"id\"  :35,\"title\":\"Limite de main 2\",\"subtitle\":\"Remplace la Limite de Main\",\"type\" :\"NewRule\",\"image\":\"new_rule_card_limit2\",\"description\":\"Si ce n'est pas votre tour de jeu, votre main ne doit compter que 2 cartes : déffaussez-vous de toute carte en excès. Durant votre tour, cette règle ne s'applique pas à vous ; une fois votre tour terminé, défaussez autant de cartes que nécessaire pour une main de 2 cartes.\"},{\"id\"  :36,\"title\":\"Inflation\",\"subtitle\":\"Effet instantané\",\"type\" :\"NewRule\",\"image\":\"new_rule_inflation\",\"description\":\"Chaque fois qu'un nombre apparaît sous forme de chiffre(s) sur une autre carte, ce nombre est augmenté d'une unité. 1 devient donc 2, tandis que un reste un. \nCela s'applique bel et bien aux Règles de Base.\"},{\"id\"  :37,\"title\":\"Limite d'Atouts 2\",\"subtitle\":\"Remplace la Limite d'Atouts\",\"type\" :\"NewRule\",\"image\":\"new_rule_keepers_limit2\",\"description\":\"En dehors de votre tour, vous ne pouvez pas avoir plus de 2 Atouts en jeu. Défaussez tout Atout en excès sur-le-champ, à votre convenance. Vous pouvez jouer des Atouts à votre tour, à condition de défausser l'excédent pour ne pas en avoir plus de 2 à la fin de votre tour.\"},{\"id\"  :38,\"title\":\"Limite d'Atouts 3\",\"subtitle\":\"Remplace la Limite d'Atouts\",\"type\" :\"NewRule\",\"image\":\"new_rule_keepers_limit3\",\"description\":\"En dehors de votre tour, vous ne pouvez pas avoir plus de 3 Atouts en jeu. Défaussez tout Atout en excès sur-le-champ, à votre convenance. Vous pouvez jouer des Atouts à votre tour, à condition de défausser l'excédent pour ne pas en avoir plus de 3 à la fin de votre tour.\"},{\"id\"  :39,\"title\":\"Limite d'Atouts 4\",\"subtitle\":\"Remplace la Limite d'Atouts\",\"type\" :\"NewRule\",\"image\":\"new_rule_keepers_limit4\",\"description\":\"En dehors de votre tour, vous ne pouvez pas avoir plus de 4 Atouts en jeu. Défaussez tout Atout en excès sur-le-champ, à votre convenance. Vous pouvez jouer des Atouts à votre tour, à condition de défausser l'excédent pour ne pas en avoir plus de 4 à la fin de votre tour.\"},{\"id\"  :40,\"title\":\"Jouez 2\",\"type\" :\"NewRule\",\"subtitle\":\"Remplace la Règle Jouer\",\"image\":\"new_rule_play2\",\"description\":\"Jouez 2 cartes par tour. \nSi votre main compte moins de 2 cartes, jouez toutes vos cartes.\"},{\"id\"  :41,\"title\":\"Jouez 3\",\"subtitle\":\"Remplace la Règle Jouer\",\"type\" :\"NewRule\",\"image\":\"new_rule_play3\",\"description\":\"Jouez 3 cartes par tour. \nSi votre main compte moins de 3 cartes, jouez toutes vos cartes.\"},{\"id\"  :42,\"title\":\"Jouez 4\",\"subtitle\":\"Remplace la Règle Jouer\",\"type\" :\"NewRule\",\"image\":\"new_rule_play4\",\"description\":\"Jouez 4 cartes par tour. \nSi votre main compte moins de 4 cartes, jouez toutes vos cartes.\"},{\"id\"  :43,\"title\":\"Jouez tout\",\"subtitle\":\"Remplace la Règle Jouer\",\"type\" :\"NewRule\",\"image\":\"new_rule_play_all\",\"description\":\"Jouez toutes les cartes de votre main à chaque tour.\"},{\"id\"  :44,\"title\":\"Jouez tout sauf 1\",\"subtitle\":\"Remplace la Règle Jouer\",\"type\" :\"NewRule\",\"image\":\"new_rule_play_all_minus1\",\"description\":\"jouez toutes les cartes de vortre main sauf 1. Si vous avez commencé sans carte en main et n'en avez pioché qu'une, piochez-en une supplémentaire.\"},{\"id\"  :45,\"title\":\"Bonus du Pauvre\",\"type\" :\"NewRule\",\"image\":\"new_rule_poov_bonus\",\"description\":\"Si un joueur a moins d'Atouts sur la table que chaque autre joueur, il pioche $number carte$s supplémentaire.\nEn cas d'égalité, personne ne profite du bonus.\"},{\"id\"  :46,\"title\":\"Première Carte Aléatoire\",\"type\" :\"NewRule\",\"image\":\"new_rule_random_first_card\",\"description\":\"La première carte que vous jouez doit être choisie au hazard dans votre main par votre voisin de gauche.\nNe tenez pas compte de cette règle si, au début de votre tour, les cartes Règles en vigeur ne vous permettent de jouer qu'une seul carte.\"},{\"id\"  :47,\"title\":\"Jeu en Aveugle\",\"type\" :\"NewRule\",\"image\":\"new_rule_random_play\",\"description\":\"Une fois pendant votre tour, vous pouvez prendre la première carte de la pioche et la jouer immédiatement.\"},{\"id\"  :48,\"title\":\"Recyclage\",\"type\" :\"NewRule\",\"image\":\"new_rule_recycling\",\"description\":\"Une fois durant votre tour, vous pouvez défausser un de vos Atouts de la table et piocher $number cartes supplémentaires.\"},{\"id\"  :49,\"title\":\"Bonus deu Riche\",\"type\" :\"NewRule\",\"image\":\"new_rule_rich_bonus\",\"description\":\"Si un joueur a plus d'Atouts sur la table que chaque autre joueur, il peut jouer $number carte$s supplémentaire.\nEn cas d'égalité, personne ne profite du bonus.\"},{\"id\"  :50,\"title\":\"Sur les Deux Tableaux\",\"type\" :\"NewRule\",\"image\":\"new_rule_two_goal\",\"description\":\"On peut jouer un second Objectif.\nAprès cela, quiconque joue un nouvel Objectif doit choisir lequel des deux Objectifs en vigeur est défaussé.\nVous gagnez la partie si vous remplissez les condition de l'un des deux Objectifs.\"},{\"id\"  :51,\"title\":\"Le Cerveau\",\"type\":\"Keeper\",\"image\":\"keeper_brain\"},{\"id\"  :52,\"title\":\"Le Pain\",\"type\":\"Keeper\",\"image\":\"keeper_bred\"},{\"id\"  :53,\"title\":\"Le Chocolat\",\"type\":\"Keeper\",\"image\":\"keeper_chocolate\"},{\"id\"  :54,\"title\":\"Les Cookies\",\"type\":\"Keeper\",\"image\":\"keeper_cookie\"},{\"id\"  :55,\"title\":\"Les Rêves\",\"type\":\"Keeper\",\"image\":\"keeper_dream\"},{\"id\"  :56,\"title\":\"L'OEil\",\"type\":\"Keeper\",\"image\":\"keeper_eye\"},{\"id\"  :57,\"title\":\"L'Amour\",\"type\":\"Keeper\",\"image\":\"keeper_love\"},{\"id\"  :58,\"title\":\"Le Lait\",\"type\":\"Keeper\",\"image\":\"keeper_milk\"},{\"id\"  :59,\"title\":\"L'Argent\",\"type\":\"Keeper\",\"image\":\"keeper_money\"},{\"id\"  :60,\"title\":\"La Lune\",\"type\":\"Keeper\",\"image\":\"keeper_moon\"},{\"id\"  :61,\"title\":\"La Musique\",\"type\":\"Keeper\",\"image\":\"keeper_music\"},{\"id\"  :62,\"title\":\"La Fête\",\"type\":\"Keeper\",\"image\":\"keeper_party\"},{\"id\"  :63,\"title\":\"La Paix\",\"type\":\"Keeper\",\"image\":\"keeper_peace\"},{\"id\"  :64,\"title\":\"La Fusée\",\"type\":\"Keeper\",\"image\":\"keeper_rocket\"},{\"id\"  :65,\"title\":\"Le Sommeil\",\"type\":\"Keeper\",\"image\":\"keeper_sleep\"},{\"id\"  :66,\"title\":\"Le Soleil\",\"type\":\"Keeper\",\"image\":\"keeper_sun\"},{\"id\"  :67,\"title\":\"Le Temps\",\"type\":\"Keeper\",\"image\":\"keeper_time\"},{\"id\"  :68,\"title\":\"Le Grille-Pain\",\"type\":\"Keeper\",\"image\":\"keeper_toaster\"},{\"id\"  :69,\"title\":\"La Télévision\",\"type\":\"Keeper\",\"image\":\"keeper_tv\"},{\"id\"  :70,\"title\":\"Le Troisième OEil\",\"type\" :\"Goal\",\"keepers1\":\"brain\",\"keepers2\":\"eye\",\"image\":\"goal_brain_eye\"},{\"id\"  :71,\"title\":\"La Cervelle (sans la Télé)\",\"type\" :\"Goal\",\"image\":\"goal_brain_not_tv\",\"description\":\"Si personne n'a La Télévision en jeu, le joueur qui a Le Cerveau sur la table l'emporte.\"},{\"id\"  :72,\"title\":\"Pain et Chocolat\",\"type\" :\"Goal\",\"keepers1\":\"bred\",\"keepers2\":\"chocolate\",\"image\":\"goal_bred_chocolate\"},{\"id\"  :73,\"title\":\"La Boulangerie\",\"type\" :\"Goal\",\"keepers1\":\"bred\",\"keepers2\":\"cookie\",\"image\":\"goal_bred_cookie\"},{\"id\"  :74,\"title\":\"Le Toast\",\"type\" :\"Goal\",\"keepers1\":\"bread\",\"keepers2\":\"toaster\",\"image\":\"goal_bred_toaster\"},{\"id\"  :75,\"title\":\"Les Cookies au Chocolat\",\"type\" :\"Goal\",\"keepers1\":\"chocolate\",\"keepers2\":\"cookie\",\"image\":\"goal_chocolate_cookie\"},{\"id\"  :76,\"title\":\"Chocolat Fondu\",\"type\" :\"Goal\",\"keepers1\":\"chocolate\",\"keepers2\":\"sun\",\"image\":\"goal_chocolate_sun\"},{\"id\"  :77,\"title\":\"Gagner au Loto\",\"type\" :\"Goal\",\"keepers1\":\"dream\",\"keepers2\":\"money\",\"image\":\"goal_dream_money\"},{\"id\"  :78,\"title\":\"La Paix dans le Monde\",\"type\" :\"Goal\",\"keepers1\":\"dream\",\"keepers2\":\"peace\",\"image\":\"goal_dream_peace\"},{\"id\"  :79,\"title\":\"Au Pays des Songes\",\"type\" :\"Goal\",\"keepers1\":\"sleep\",\"keepers2\":\"dream\",\"image\":\"goal_sleep_dream\"},{\"id\"  :80,\"title\":\"L'Amour est Aveugle\",\"type\" :\"Goal\",\"keepers1\":\"eye\",\"keepers2\":\"love\",\"image\":\"goal_eye_love\"},{\"id\"  :81,\"title\":\"5 Atouts\",\"type\" :\"Goal\",\"image\":\"goal_five_keepers\",\"description\":\"Si au moins un joueurs dispose de 5 Atouts sur la table, celui qui a le plus d'Atouts en jeu l'emporte. /nEn cas d'égalité, la partie se prolonge jusqu'à ce qu'un vainqueur se démarque.\"},{\"id\"  :82,\"title\":\"De Coeur et d'Esprit\",\"type\" :\"Goal\",\"keepers1\":\"love\",\"keepers2\":\"brain\",\"image\":\"goal_love_brain\"},{\"id\"  :83,\"title\":\"Le Lait Chocolaté\",\"type\" :\"Goal\",\"keepers1\":\"chocolate\",\"keepers2\":\"milk\",\"image\":\"goal_chocolate_milk\"},{\"id\"  :84,\"title\":\"Les Biscuits Trempés\",\"type\" :\"Goal\",\"keepers1\":\"milk\",\"keepers2\":\"cookie\",\"image\":\"goal_milk_cookie\"},{\"id\"  :85,\"title\":\"L'Amour n'a pas de prix\",\"type\" :\"Goal\",\"keepers1\":\"money\",\"keepers2\":\"love\",\"image\":\"goal_money_love\"},{\"id\"  :86,\"title\":\"Le Temps c'est de l'Argent\",\"type\" :\"Goal\",\"keepers1\":\"time\",\"keepers2\":\"money\",\"image\":\"goal_money_time\"},{\"id\"  :87,\"title\":\"Le Jour et la Nuit\",\"type\" :\"Goal\",\"keepers1\":\"moon\",\"keepers2\":\"sun\",\"image\":\"goal_moon_sun\"},{\"id\"  :88,\"title\":\"À fond la musique !\",\"type\" :\"Goal\",\"keepers1\":\"music\",\"keepers2\":\"party\",\"image\":\"goal_music_party\"},{\"id\"  :89,\"title\":\"Les Génériques de mon Enfance\",\"type\" :\"Goal\",\"keepers1\":\"music\",\"keepers2\":\"tv\",\"image\":\"goal_music_tv\"},{\"id\"  :90,\"title\":\"Les Petits Fours\",\"subtitle\":\"La Fête + au moins un Atout alimentaire.\",\"type\" :\"Goal\",\"image\":\"goal_party_food\"},{\"id\"  :91,\"title\":\"C'est l'heure de la Fête !\",\"type\" :\"Goal\",\"keepers1\":\"party\",\"keepers2\":\"time\",\"image\":\"goal_party_time\"},{\"id\"  :92,\"title\":\"Les Babas Cools\",\"type\" :\"Goal\",\"keepers1\":\"peace\",\"keepers2\":\"love\",\"image\":\"goal_peace_love\"},{\"id\"  :93,\"title\":\"L'Ingénieur\",\"type\" :\"Goal\",\"keepers1\":\"rocket\",\"keepers2\":\"brain\",\"image\":\"goal_rocket_brain\"},{\"id\"  :94,\"title\":\"On a Marché...\",\"type\" :\"Goal\",\"keepers1\":\"rocket\",\"keepers2\":\"moon\",\"image\":\"goal_rocket_moon\"},{\"id\"  :95,\"title\":\"L'Heure de se Coucher\",\"type\" :\"Goal\",\"keepers1\":\"sleep\",\"keepers2\":\"time\",\"image\":\"goal_sleep_time\"},{\"id\"  :96,\"title\":\"Berceuse\",\"type\" :\"Goal\",\"keepers1\":\"sleep\",\"keepers2\":\"music\",\"image\":\"goal_sleep_music\"},{\"id\"  :97,\"title\":\"Rêve Éveillé\",\"type\" :\"Goal\",\"keepers1\":\"sun\",\"keepers2\":\"dream\",\"image\":\"goal_sun_dream\"},{\"id\"  :98,\"title\":\"10 Cartes en Main\",\"type\" :\"Goal\",\"description\":\"Si au moins un joueurs a 10 cartes ou plus en main, celui qui en a le plus l'emporte.\nEn cas d'égalité, la partie se prolonge jusqu'à ce qu'un vainqueur se démarque.\",\"image\":\"goal_ten_cards\"},{\"id\"  :99,\"title\":\"L'Éléctroménager\",\"type\" :\"Goal\",\"keepers1\":\"toaster\",\"keepers2\":\"tv\",\"image\":\"goal_toaster_tv\"}]";
            return JsonConvert.DeserializeObject<List<Card>>(json).ToArray();
        }
       

        private void Test_Clicked(object sender, EventArgs e)
        {
            //MoveCard(CardsViews[1], Test, player_0);
            CardsViews[1].AnimTurnCard(test);
            test = !test;
        }
    }
}