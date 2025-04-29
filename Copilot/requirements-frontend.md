# Wymagania Frontend (Angular) dla projektu SouthAmp

## Technologie

- Angular 17+
- RxJS
- Angular Material (lub inna biblioteka UI)
- ngx-translate (wielojęzyczność)
- Angular Router
- Interceptory HTTP (autoryzacja, obsługa błędów)
- Guards (ochrona tras)
- Formularze reaktywne
- Integracja z backendem przez REST API

## Struktura aplikacji

### Moduły

- Auth (logowanie, rejestracja, reset hasła)
- User (profil, ustawienia, role)
- Hotels (lista, szczegóły, zarządzanie)
- Rooms (zarządzanie pokojami/ofertami)
- Reservations (rezerwacje, historia, anulowanie)
- Payments (płatności, historia transakcji)
- Reviews (opinie, oceny)
- Admin (panel administracyjny)
- Notifications (powiadomienia)
- Reports (zgłaszanie problemów)
- Discounts (kody rabatowe)
- Shared (komponenty wspólne, serwisy, modele)

## Widoki i funkcjonalności

### 1. Zarządzanie użytkownikami

- Rejestracja (gość, właściciel, admin)
- Logowanie, wylogowanie (JWT/cookie)
- Reset i zmiana hasła
- Edycja profilu
- Weryfikacja e-mail (opcjonalnie SMS)
- Obsługa ról (gość, właściciel, admin)
- Ochrona tras (guards)

### 2. Zarządzanie hotelami/usługami

- Dodawanie, edycja, usuwanie hotelu/usługi (dla właściciela)
- Lista hoteli/usług (dla wszystkich)
- Wyszukiwanie (nazwa, lokalizacja, kategorie, dostępność)
- Filtrowanie i sortowanie (cena, popularność, ocena)
- Szczegóły hotelu/usługi (opis, zdjęcia, pokoje, oceny)

### 3. Zarządzanie pokojami/ofertami

- Dodawanie, edycja, usuwanie pokoi/ofert (dla właściciela)
- Ustawianie cen, dostępności (kalendarz)
- Zarządzanie zdjęciami pokoi

### 4. Rezerwacje

- Tworzenie rezerwacji (gość)
- Podgląd własnych rezerwacji
- Anulowanie rezerwacji
- Zmiana terminu (jeśli polityka pozwala)
- Sprawdzanie dostępności przed rezerwacją

### 5. Płatności

- Tworzenie sesji płatności (np. Stripe)
- Potwierdzenie płatności
- Zwroty (anulowanie rezerwacji)
- Historia transakcji użytkownika
- Raporty sprzedaży dla właścicieli

### 6. Oceny i opinie

- Dodawanie oceny/opinii po rezerwacji
- Edycja/usuwanie własnych opinii
- Podgląd średniej oceny hotelu/usługi
- Filtrowanie ofert po ocenach

### 7. Panel administratora

- Zarządzanie użytkownikami (ban, aktywacja, usuwanie)
- Zarządzanie hotelami/usługami (moderacja zgłoszonych ofert)
- Zarządzanie opiniami (moderacja zgłoszonych opinii)
- Dostęp do wszystkich transakcji

### 8. Powiadomienia

- Powiadomienia e-mail (rejestracja, rezerwacja, anulowanie, zmiana terminu)
- Powiadomienia systemowe (np. snackbar, toast)
- (Opcjonalnie) WebSocket/SignalR

### 9. Zgłoszenia i wsparcie

- Formularz zgłoszenia problemu z rezerwacją/usługą
- Podgląd i odpowiedzi admina

### 10. Promocje i kody rabatowe

- Tworzenie kodów rabatowych (właściciel/admin)
- Weryfikacja kodu przy rezerwacji
- Limity i daty ważności kodów

### 11. Audyt i bezpieczeństwo

- Logi aktywności użytkownika (np. logowania, rezerwacje)
- Audyt zmian w ofertach/hotelach
- Ochrona endpointów (autoryzacja, rate limiting)

### 12. Lokalizacje

- Wyszukiwanie po mieście/kraju
- Obsługa mapy (np. Google Maps, leaflet)
- Przypisywanie lokalizacji do hotelu/usługi

## Integracja z backendem

- Każdy serwis Angulara powinien korzystać z endpointów REST API (zgodnie z dokumentacją Swagger)
- Obsługa JWT (przechowywanie tokenu, odświeżanie, przekazywanie w nagłówkach)
- Globalna obsługa błędów (interceptor)
- Walidacja danych po stronie klienta (formularze) i obsługa błędów z backendu

## Wymagania dodatkowe

- Responsywność (mobile first)
- Wielojęzyczność (min. PL/EN)
- Testy jednostkowe (np. Jasmine/Karma)
- Testy E2E (np. Cypress)
- Dokumentacja kodu i uruchomienia projektu
