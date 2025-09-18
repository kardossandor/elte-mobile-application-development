# Horoscope App

The goal of the application is to provide users with daily horoscope for themselves and their friends. Users will register and log in using **Firebase Authentication**. Users **can add friends** by saving their **nickname, birthday, and a photo**. Based on the stored birthday, the app automatically calculates the zodiac sign. The data, including **profile photos stored in Base64 format**, will be saved securely in **Firebase Firestore**. Through the **system’s native share panel**, users will also be **able to share** the daily horoscope text of a specific zodiac sign.

The app will fetch daily horoscope texts from an **external API** ([example call](https://api.api-ninjas.com/v1/horoscope?zodiac=aries)), **cache them locally**, and display them inside the app. Users will also receive **local notifications** at a **configurable time**, reminding them to check their daily horoscope or that of their friends. **Tapping a notification** will open the application Dashboard.

Main screens will include:

* **Login/Onboarding** (Firebase sign-in)
* **Home/Dashboard** (list of friends and today’s horoscopes)
* **Friend Detail** (photo, nickname, today’s horoscope, horoscope history)
* **Add/Edit Friend** (form with photo upload and birthday picker)
* **Settings** (notification time, logout)

For architecture, the application will follow an **MV structure** initially (Pages + Services), with the option to extend to **MVVM** if more time allows. Service layers will manage Firebase access, horoscope API integration, local cache, and notification scheduling.
