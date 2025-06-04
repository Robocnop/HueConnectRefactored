# 📗 **HueConnectRefactored** v1.0.0

> **Basé sur le plugin de** : *Zerox_Hue*  
> **Refactorisé par** : *Robocnop*  
> **Objectif** : Faire fonctionner HueConnect avec les versions actuelles de NovaLife et le rendre plus léger en taille.

---

## 🔧 Fonctionnalités

- Envoie un message, **uniquement aux administrateurs**, lorsqu’un joueur se connecte.
- Inclut dans la notification :
  - Nom complet du joueur
  - Nom Steam
  - ID Steam
  - Indication si le joueur est admin (et son niveau d’admin)

---

## ⬇️ Installation

1. **Téléchargez** le fichier `HueConnectRefactored.dll`.
2. **Placez** `HueConnectRefactored.dll` dans le dossier `Plugins` de votre serveur NovaLife.
3. **Lancez** votre serveur, puis **éditez** le fichier `config.json` généré à côté de la DLL (dans le répertoire ayant le même nom) pour y ajouter votre webhook Discord :
   ```json
   {
     "LoginWebhookUrl": "https://discord.com/api/webhooks/…"
   }
4. Redémarrez le serveur pour appliquer la configuration.
5. Profitez : désormais, chaque connexion de joueur sera notifiée aux admins (et sur Discord, si configuré).
