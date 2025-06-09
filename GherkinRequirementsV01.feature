Feature: Find the best nuts in the forest
As a hungry squirrel
I want to find the best and ripest nuts in the forest
So that I can optimize my nut-gathering efficiency

    Scenario: 01 - If I request information about a tree the correct tree is returned
        Given the forest has an "oak" tree with the name "oak-1" and "22" "ripe" nuts
        When I request information about the tree with the name "oak-1"
        Then the response should include the tree named "oak-1" with "22" "ripe" nuts

    