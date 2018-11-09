import React, { Component } from 'react';
import { ButtonToolbar, DropdownButton, MenuItem } from 'react-bootstrap';

class Filter extends Component {
  sortDivisions() {
    const { allDivisions } = this.props;
    allDivisions.sort((a, b) => a.tier - b.tier || a.firstSeason - b.firstSeason);
  };

  render() {
    const { allSeasons, allDivisions, selectedDivision, selectedSeason, selectDivision, selectSeason } = this.props;
    
    this.sortDivisions();
    
    return (
      <ButtonToolbar>
        <DropdownButton title="Division" id="DivisionSelect">
        {
          allDivisions.map(d => 
            <MenuItem key={`${d.name} - ${d.tier} - ${d.firstSeason}`}
              eventKey={d} 
              className={d.name === selectedDivision ? "active" : ""}
              onSelect={(d) => selectDivision(d)}
            >
              {d.name}
            </MenuItem>)
        }
        </DropdownButton>
        <DropdownButton title="Season" id="SeasonSelect">
        {
          allSeasons.map(s =>
            <MenuItem key={s} eventKey={s}
              className={s === selectedSeason ? "active" : ""}
              onSelect={(s) => selectSeason(s)}
            >
              {s}
            </MenuItem>)
        }
        </DropdownButton>
      </ButtonToolbar>
    );
  }
}

export default Filter;
