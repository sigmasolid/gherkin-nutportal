Feature: Nutty API features
As a hungry squirrel
I want to find the best and ripest nuts in the forest
So that I can optimize my nut-gathering efficiency

    Scenario: 01 - If I request information about a tree the correct tree is returned
        Given the forest has an "oak" tree with the name "oak-1" and 22 "ripe" nuts
        When I request information about the tree with the name "oak-1"
        Then the response should include the tree ID "oak-1" with 22 "ripe" nuts

    Scenario: 02 - If I request information about a tree that does not exist
        Given the forest has no trees
        When I request information about a tree with the name "oak-2"
        Then the response should indicate that the tree does not exist

    