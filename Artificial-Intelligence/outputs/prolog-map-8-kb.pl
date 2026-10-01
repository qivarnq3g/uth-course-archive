% Knowledge base for the fixed Wumpus World map started with start(8).
% Coordinates use X=1..4 from left to right and Y=1..4 from bottom to top.
% This file is a derived output. The original wumpus.qlf is not modified.

:- discontiguous observation/5.
:- discontiguous event/3.

map_id(8).
grid_size(4, 4).
cave_entrance(0, 0).
start_cell(1, 1).

% Ground truth read from map_data(8, Wumpus, Gold, Pits).
wumpus(3, 1).
gold(3, 3).
pit(1, 3).
pit(2, 2).
pit(4, 2).

inside(X, Y) :-
    between(1, 4, X),
    between(1, 4, Y).

adjacent(X, Y, NX, NY) :-
    inside(X, Y),
    inside(NX, NY),
    (   NX is X + 1, NY =:= Y
    ;   NX is X - 1, NY =:= Y
    ;   NX =:= X, NY is Y + 1
    ;   NX =:= X, NY is Y - 1
    ).

hazard(X, Y) :- pit(X, Y).
hazard(X, Y) :- wumpus(X, Y).

safe(X, Y) :-
    inside(X, Y),
    \+ hazard(X, Y).

% Percept semantics while the Wumpus is alive.
breeze(X, Y) :- adjacent(X, Y, PX, PY), pit(PX, PY).
stench(X, Y) :- adjacent(X, Y, WX, WY), wumpus(WX, WY).
glitter(X, Y) :- gold(X, Y).

% The successful observed run. Percepts are post-action percepts.
% Scream is an action event, recorded separately at event/3.
observation(0, climbdown, position(1, 1), [], 0).
observation(1, goright, position(2, 1), [breeze, stench], -1).
event(2, shootright, scream).
observation(2, shootright, position(2, 1), [breeze], -11).
observation(3, goright, position(3, 1), [], -12).
observation(4, goup, position(3, 2), [breeze], -13).
observation(5, goup, position(3, 3), [glitter], -14).
event(6, grab, gold_acquired).
observation(6, grab, position(3, 3), [glitter], 86).
observation(7, godown, position(3, 2), [breeze], 85).
observation(8, godown, position(3, 1), [], 84).
observation(9, goleft, position(2, 1), [breeze], 83).
observation(10, goleft, position(1, 1), [], 82).
event(11, climbup, mission_completed).
observation(11, climbup, position(0, 0), [], 1082).

% Expected initial percepts for every cave cell, including hazardous cells.
% Percepts on hazardous cells are theoretical: entering such a cell ends play.
expected_percepts(1, 1, []).
expected_percepts(2, 1, [breeze, stench]).
expected_percepts(3, 1, []).
expected_percepts(4, 1, [breeze, stench]).
expected_percepts(1, 2, [breeze]).
expected_percepts(2, 2, []).
expected_percepts(3, 2, [breeze, stench]).
expected_percepts(4, 2, []).
expected_percepts(1, 3, []).
expected_percepts(2, 3, [breeze]).
expected_percepts(3, 3, [glitter]).
expected_percepts(4, 3, [breeze]).
expected_percepts(1, 4, [breeze]).
expected_percepts(2, 4, []).
expected_percepts(3, 4, []).
expected_percepts(4, 4, []).
