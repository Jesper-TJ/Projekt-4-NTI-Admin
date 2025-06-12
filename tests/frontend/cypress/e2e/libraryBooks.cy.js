describe('Library Books Page', () => {
    beforeEach(() => {
      cy.visit('http://localhost:3000/#/library'); // Adjust route as needed
    });
    
    /* 
        Passes
    */
    it('renders the page with all key components', () => {
      cy.get('.library-container').should('be.visible');
      cy.get('.header').should('contain.text', 'Library Books');
      cy.get('.search-bar').should('be.visible');
      cy.get('.v-data-table').should('be.visible');
    });

    /* 
        Passes
    */
        it('filters books based on search input', () => {
          const searchText = 'C# in Depth'; // Replace with a relevant book title from your test data
        
          // Intercept the fetchBooks API call
          cy.intercept('GET', '/api/Books/all').as('fetchBooks');
        
          // Wait for the fetchBooks API call to complete
          cy.wait('@fetchBooks');
        
          // Wait for the data table to display rows
          cy.get('.v-data-table tbody tr', { timeout: 10000 }).should('exist');
        
          // Type into the search bar
          cy.get('.search-bar input').type(searchText, { delay: 100 }); // --< skibidi need to have a W delay on 100 ms ^-^
          
          cy.contains('Loading titles ...', { timeout: 10000 }).should('not.exist');

          // Ensure the search filters the rows
          cy.get('.v-data-table tbody tr')
            .should('have.length.greaterThan', 0) // Ensure there are rows after filtering
            .each(($row) => {
              // Break the chain here to avoid re-querying detached elements
              cy.wrap($row).find('td:first').invoke('text').then((text) => {
                expect(text.trim()).to.contain(searchText);
              });
            });
        });
        
    /* 
        Passes
    */
        it('navigates to book details on row click', () => {
          // Intercept API call
          cy.intercept('GET', '/api/Books/all').as('fetchBooks');
        
          // Visit the page and wait for the API call
          cy.wait('@fetchBooks');
        
          // Wait for rows to load
          cy.get('.v-data-table tbody tr', { timeout: 10000 }).should('exist');
        
          // Click the first row
          cy.get('.v-data-table tbody tr').first().click(); 
        
          // Validate navigation to the expected URL
          cy.url().should('include', `/book/1`); // Adjust URL if needed
        });
        
  
    /* 
        Passes
    */
    it('shows loading text while fetching data', () => {
        // Mock delay in your backend for testing
        cy.intercept('GET', '/api/library-books', (req) => {
          req.on('response', (res) => {
            res.delay(1000); // Simulate delay
          });
        });
      
        cy.visit('http://localhost:3000/#/library');
      
        // Wait for the progress indicator to appear
        cy.get('.v-data-table')
          .should('be.visible')
          .and('contain.text', 'Loading titles ...');
      });

      it('preserves the URL and state after reload', () => {
        // Get the current location
        cy.location().then((loc) => {
          const pageUrl = loc.href; // Capture the current URL
      
          // Reload the page
          cy.reload();
      
          // Assert the URL remains the same after reload
          cy.url().should('eq', pageUrl);
      
          // Optionally, validate specific page elements or state
          cy.get('.library-container').should('be.visible'); // Ensure main container is still visible
          cy.get('.v-data-table').should('be.visible'); // Ensure data table is still rendered
        });
      });
      
      
  });
  