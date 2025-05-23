function getAll() {
    fetch('http://localhost:21271/api/users') // Pravi GET zahtev da dobavi sve knjige sa servera
      .then(response => {
        if (!response.ok) {
          // Ako se vrati statusni kod koji nije iz 2xx, tretiraj kao grešku
          throw new Error('Request failed. Status: ' + response.status)
        }
        return response.json()
      })
      .then(users => ispisiUsers(users))  // Ako su podaci ispravni, prikaži ih u HTMLu
      .catch(error => {                  // Ako podaci nisu ispravni, sakrij tabelu i prikaži poruku o grešci
        console.error('Error:', error.message)
        // Sakrij tabelu
        let table = document.querySelector('table')
        if (table) {
          table.style.display = 'none'
        }
        // Prikaži poruku o grešci
        alert('An error occurred while loading the data. Please try again.')
      })
  }


  
function ispisiUsers(nizUsera){
    let tabela = document.querySelector('#usersBody')
    tabela.innerHTML = ''

    
    for(let user of nizUsera){
        let noviRed = tabela.insertRow()
    
        let idCell = noviRed.insertCell()
        idCell.textContent = user.id

        let korisnickoImeCell = noviRed.insertCell()
        korisnickoImeCell.textContent = user.korisnickoIme

        let imeCell = noviRed.insertCell()
        imeCell.textContent = user.ime

        let prezimeCell = noviRed.insertCell()
        prezimeCell.textContent = user.prezime

        let datumCell =noviRed.insertCell()
        let originalniDatum = new Date(user.datumRodjenja); // Pretvara string u Date objekat
        let dan = String(originalniDatum.getDate()).padStart(2, '0'); // Dodaje 0 ispred jednocifrenih dana
        let mesec = String(originalniDatum.getMonth() + 1).padStart(2, '0'); // Meseci su 0-indeksirani, pa dodajemo 1
        let godina = originalniDatum.getFullYear(); // Dobija punu godinu
        datumCell.textContent = `${dan}/${mesec}/${godina}`; // Formatira datum u dd/MM/yyyy

    }
}

document.addEventListener('DOMContentLoaded', getAll)